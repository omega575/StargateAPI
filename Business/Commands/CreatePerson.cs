namespace StargateAPI.Business.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor(StargateContext context) : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context = context;

        public Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            if (person is not null) throw new BadHttpRequestException("Bad Request");

            return Task.CompletedTask;
        }
    }

    public class CreatePersonHandler(StargateContext context) : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context = context;

        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {

            var newPerson = new Person()
            {
                Name = request.Name
            };

            await _context.People.AddAsync(newPerson, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreatePersonResult()
            {
                Id = newPerson.Id
            };

        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
