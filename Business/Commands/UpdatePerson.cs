namespace StargateAPI.Business.Commands;

public class UpdatePerson : IRequest<UpdatePersonResult>
{
    public required int Id { get; set; }
    public string? Name { get; set; }
}

// We don't need to pre-process this request as we want to update an existing person if they are found

public class UpdatePersonHandler(StargateContext context) : IRequestHandler<UpdatePerson, UpdatePersonResult>
{
    private readonly StargateContext _context = context;

    public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
    {
        Person? person = await _context.People.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new BadHttpRequestException("Bad Request");

        // if the name is provided update it, otherwise keep it the same
        person.Name = request.Name ?? person.Name;

        // TODO: update Astronaut Details and Duties

        // EF Core should be aware of changes made to found entity and will save those changes with 'SaveChangesAsync()'
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdatePersonResult { Id = person.Id };
    }
}

public class UpdatePersonResult : BaseResponse
{
    public int Id { get; set; }
}
