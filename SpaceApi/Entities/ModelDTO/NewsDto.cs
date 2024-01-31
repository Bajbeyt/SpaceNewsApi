namespace Entities.ModelDTO;

public record NewsDto(int Id,string Title,string Content,DateTime ReleasDate,DateTime DateOfUpdate,string AuthorName);