using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyIMDB.Services.Helpers
{
    public class SeedService : ISeedService
    {
        private readonly ITmdbService service;

        public SeedService(ITmdbService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }
        public void Seed()
        {
            service.Seed();
        }
    }
}
