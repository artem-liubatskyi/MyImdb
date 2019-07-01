using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyIMDB.Data;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TmdbClient;
using TmdbClient.ApiModels;

namespace MyIMDB.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly ITmdbClient client;
        private readonly IMapper mapper;
        private readonly ImdbContext context;

        public TmdbService(ITmdbClient client, IMapper mapper, ImdbContext context)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private string ParseCountryName(string placeOfBirth)
        {

            var countryName = placeOfBirth.Trim().Split(' ').Last();
            if (countryName.Contains("Republic") || countryName.Contains("Kingdom"))
            {
                var words = placeOfBirth.Split(' ');
                countryName = $"{words[words.Count() - 2]} {words[words.Count() - 1]}";
            }
            if (countryName == "United States of America" || countryName == "U.S.A." || countryName == "NY"
                || countryName == "US" || countryName == "America" || countryName == "York")
                countryName = "USA";
            if (countryName == "England" || countryName == "United Kingdom")
                countryName = "UK";
            return countryName;
        }
        private async Task AddNecessaryPersonsAsync(List<Person> persons)
        {
            foreach (var person in persons)
            {
                var personEntity = await context.MoviePersons.FirstOrDefaultAsync(x => x.FullName == person.Name);

                if (personEntity == null)
                {
                    var entity = mapper.Map<Person, MoviePerson>(person);

                    if (person.Place_of_birth != null)
                    {
                        entity.CountryId = context.Countries.First(x => x.Name == ParseCountryName(person.Place_of_birth)).Id;
                    }
                    var gender = context.Genders.FirstOrDefault(x => x.Id == person.Gender);
                    if (gender != null)
                        entity.GenderId = gender.Id;
                    await context.MoviePersons.AddAsync(entity);
                }
            }
            context.SaveChanges();
        }
        private async Task AddNecessaryCountriesAsync(List<Person> persons)
        {
            foreach (var person in persons)
            {
                if (person.Place_of_birth == null)
                    continue;

                var countryName = ParseCountryName(person.Place_of_birth);

                var countryEntity = await context.Countries.FirstOrDefaultAsync(x => x.Name == countryName);

                if (countryEntity == null)
                {
                    context.Countries.Add(new Country
                    {
                        Name = countryName
                    });
                    context.SaveChanges();
                }
            }

        }
        private async Task AddNecessaryGenresAsync(List<TmdbClient.ApiModels.Genre> genres)
        {
            foreach (var genre in genres)
            {
                var genreEntity = await context.Genres.FirstOrDefaultAsync(x => x.Title == genre.Name);
                if (genreEntity == null)
                {
                    context.Genres.Add(new MyIMDB.Data.Entities.Genre
                    {
                        Title = genre.Name
                    });
                    context.SaveChanges();
                }
            }
        }
        private async Task AddNecessaryCountriesAsync(List<ProductionCountry> countries)
        {
            foreach (var country in countries)
            {
                var countryEntity = context.Countries.FirstOrDefault(x => x.Name == ParseCountryName(country.Name));

                if (countryEntity == null)
                {
                    await context.Countries.AddAsync(new Country
                    {
                        Name = country.Name
                    });
                }
            }
            await context.SaveChangesAsync();
        }
        private async Task AddReferencesWithPersons(long movieId, List<Person> persons, long referenceTypeId)
        {
            var names = persons.Select(x => x.Name).Distinct();

            foreach (var name in names)
            {
                var person = await context.MoviePersons.FirstOrDefaultAsync(x => x.FullName == name);

                await context.MoviePersonsMovies.AddAsync(new MoviePersonsMovies
                {
                    MovieId = movieId,
                    MoviePersonId = person.Id,
                    MoviePersonTypeId = referenceTypeId
                });
            }
            await context.SaveChangesAsync();
        }
        private async Task AddReferencesWithCountries(long movieId, List<ProductionCountry> countries)
        {
            foreach (var country in countries)
            {
                var countryEntity = await context.Countries.FirstOrDefaultAsync(x => x.Name == ParseCountryName(country.Name));
                await context.MoviesCountries.AddAsync(new MoviesCountries
                {
                    MovieId = movieId,
                    CountryId = countryEntity.Id
                });
            }
            await context.SaveChangesAsync();
        }
        private async Task AddReferencesWithGenres(long movieId, List<TmdbClient.ApiModels.Genre> genres)
        {
            foreach (var genre in genres)
            {
                var genreEntity = await context.Genres.FirstOrDefaultAsync(x => x.Title == genre.Name);
                await context.MoviesGenres.AddAsync(new MoviesGenres
                {
                    MovieId = movieId,
                    GenreId = genreEntity.Id
                });
            }
            await context.SaveChangesAsync();
        }
        public async Task AddMovie(string title)
        {
            var movie = await GetMovieAsync(title);
            if (movie == null)
                return;

            IDbContextTransaction transaction = null;

            try
            {
                context.Database.OpenConnection();
                transaction = context.Database.BeginTransaction();

                context.Movies.Add(mapper.Map<TmdbMovie, Movie>(movie));
                context.SaveChanges();

                var movieEntity = await context.Movies.FirstOrDefaultAsync(x => x.Title == movie.Original_title);

                var credits = await GetMovieCredits(movie.Id);

                var stars = await GetStars(credits);
                await AddNecessaryCountriesAsync(stars);

                var directors = await GetDirecters(credits);
                await AddNecessaryCountriesAsync(directors);

                await AddNecessaryCountriesAsync(movie.Production_countries);
                await AddNecessaryGenresAsync(movie.Genres);
                await AddNecessaryPersonsAsync(stars);
                await AddNecessaryPersonsAsync(directors);

                var starType = await context.MoviePersonsType.FirstOrDefaultAsync(x => x.Type == Constants.StarType);
                var directorType = await context.MoviePersonsType.FirstOrDefaultAsync(x => x.Type == Constants.DirectorType);

                await AddReferencesWithPersons(movieEntity.Id, stars, starType.Id);
                await AddReferencesWithPersons(movieEntity.Id, directors, directorType.Id);

                await AddReferencesWithCountries(movieEntity.Id, movie.Production_countries);
                await AddReferencesWithGenres(movieEntity.Id, movie.Genres);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }
        public async Task<TmdbMovie> GetMovieAsync(string title)
        {
            var movieSearchResults = await client.FindMovieAsync(title);
            TmdbMovie movie = null;
            if (movieSearchResults != null && movieSearchResults.results.Any())
                movie = await client.GetMovieByIdAsync(movieSearchResults.results.First().Id);
            if (movie != null && movie.Poster_path != null)
                movie.Poster_path = TmdbClient.Settings.GetImageUrl(movie.Poster_path);
            return movie;
        }
        public async Task<Person> GetPersonAsync(long personId)
        {
            var person = await client.GetPersonByIdAsync(personId);
            if (person != null && person.Profile_path != null)
                person.Profile_path =  TmdbClient.Settings.GetImageUrl(person.Profile_path);
            return person;
        }
        public async Task<Credits> GetMovieCredits(long movieId)
        {
            return await client.GetCreditsAsync(movieId);
        }
        public async Task<List<Person>> GetStars(Credits credits)
        {
            List<Person> stars = new List<Person>();
            foreach (var star in credits.Cast.Take(5))
            {
                var person = await GetPersonAsync(star.Id);

                if (person != null)
                    stars.Add(person);
            }
            return stars;
        }
        public async Task<List<Person>> GetDirecters(Credits credits)
        {
            List<Person> directors = new List<Person>();

            foreach (var director in credits.Crew)
            {
                if (director.Job != Constants.DirectorType)
                    continue;

                var person = await GetPersonAsync(director.Id);

                if (person != null)
                    directors.Add(person);
            }
            return directors;
        }
    }
}