namespace StargateAPI.Business.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;

        public GetAstronautDutiesByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {

            var result = new GetAstronautDutiesByNameResult();

            var person = await _context.People.Include(x => x.AstronautDetail).Include(x => x.AstronautDuties).FirstOrDefaultAsync(x => x.Name.Equals(request.Name), cancellationToken);
            if (person is not null)
            {
                result.Person = new PersonAstronaut
                {
                    Name = person.Name,
                    PersonId = person.Id,
                    CurrentRank = person.AstronautDetail?.CurrentRank ?? string.Empty,
                    CurrentDutyTitle = person.AstronautDetail?.CurrentDutyTitle ?? string.Empty,
                    CareerStartDate = person.AstronautDetail?.CareerStartDate,
                    CareerEndDate = person.AstronautDetail?.CareerEndDate,
                };

                if (person.AstronautDuties is not null)
                {
                    // The reference to person is cyclical
                    result.AstronautDuties = [.. person.AstronautDuties.Select(x => new AstronautDuty {
                        Id = x.Id,
                        PersonId = x.PersonId,
                        Rank = x.Rank,
                        DutyTitle = x.DutyTitle,
                        DutyEndDate = x.DutyEndDate,
                        DutyStartDate = x.DutyStartDate,
                    })];
                }
            }

            return result;
        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
