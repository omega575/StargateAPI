namespace StargateAPI.Business.Queries
{
    public class GetPeople : IRequest<GetPeopleResult>
    {
        public int? Id { get; set; }
    }

    public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
    {
        public readonly StargateContext _context;
        public GetPeopleHandler(StargateContext context)
        {
            _context = context;
        }
        public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
        {
            var result = new GetPeopleResult();

            var people = await _context.People.Include(x => x.AstronautDetail).ToListAsync(cancellationToken);

            if (request.Id.HasValue)
            {
                // TODO: add error handling
                people = [.. people.Where(x => x.Id == request.Id)];
            }

            result.People = [.. people.Select(x => new PersonAstronaut
            {
                PersonId = x.Id,
                Name = x.Name,
                CurrentRank = x.AstronautDetail?.CurrentRank ?? string.Empty,
                CurrentDutyTitle = x.AstronautDetail?.CurrentDutyTitle ?? string.Empty,
                CareerStartDate = x.AstronautDetail?.CareerStartDate,
                CareerEndDate = x.AstronautDetail?.CareerEndDate,
            })];

            return result;
        }
    }

    public class GetPeopleResult : BaseResponse
    {
        public List<PersonAstronaut> People { get; set; } = [];

    }
}
