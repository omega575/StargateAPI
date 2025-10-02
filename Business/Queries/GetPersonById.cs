namespace StargateAPI.Business.Queries
{
    public class GetPersonById : IRequest<GetPersonByIdResult>
    {
        public required int Id { get; set; }
    }

    public class GetPersonByIdHandler : IRequestHandler<GetPersonById, GetPersonByIdResult>
    {
        private readonly StargateContext _context;
        public GetPersonByIdHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetPersonByIdResult> Handle(GetPersonById request, CancellationToken cancellationToken)
        {
            var result = new GetPersonByIdResult();

            var person = await _context.People.Include(x => x.AstronautDetail).SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

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

    public class GetPersonByIdResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
