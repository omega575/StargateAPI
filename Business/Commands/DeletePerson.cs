namespace StargateAPI.Business.Commands;

public class DeletePerson : IRequest<DeletePersonResult>
{
    public required int Id { get; set; }
}


public class DeletePersonHandler(StargateContext context) : IRequestHandler<DeletePerson, DeletePersonResult>
{
    private readonly StargateContext _context = context;

    public async Task<DeletePersonResult> Handle(DeletePerson request, CancellationToken cancellationToken)
    {
        Person? person = await _context.People.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new BadHttpRequestException("Bad Request");

        _context.People.Remove(person);

        await _context.SaveChangesAsync(cancellationToken);

        return new DeletePersonResult { };
    }
}

public class DeletePersonResult : BaseResponse
{
    
}
