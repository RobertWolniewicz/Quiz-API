using Microsoft.Extensions.Options;
using Quiz_API.Entity;
using Sieve.Models;
using Sieve.Services;

namespace Quiz_API.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }
        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Question>(q => q.Categorys)
                .CanFilter()
                .CanSort();
            mapper.Property<Question>(q => q.Points)
               .CanFilter()
               .CanSort();
            mapper.Property<Category>(c => c.Name)
               .CanFilter()
               .CanSort();

            return mapper;
        }
    }
}
