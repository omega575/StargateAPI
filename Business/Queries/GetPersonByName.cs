namespace StargateAPI.Business.Queries
{
    public class GetPersonByName : IRequest<GetPersonByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
    {
        private readonly StargateContext _context;
        public GetPersonByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
        {
            var result = new GetPersonByNameResult();

            var person = await _context.People.Include(x => x.AstronautDetail).SingleOrDefaultAsync(x => x.Name.Equals(request.Name), cancellationToken);

            if (person is not null)
            {
                result.Person = new PersonAstronaut
                {
                    PersonId = person.Id,
                    Name = person.Name,
                    CurrentRank = person.AstronautDetail?.CurrentRank ?? string.Empty,
                    CurrentDutyTitle = person.AstronautDetail?.CurrentDutyTitle ?? string.Empty,
                    CareerStartDate = person.AstronautDetail?.CareerStartDate,
                    CareerEndDate = person.AstronautDetail?.CareerEndDate,
                };
            }

            return result;
        }
    }

    public class GetPersonByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
