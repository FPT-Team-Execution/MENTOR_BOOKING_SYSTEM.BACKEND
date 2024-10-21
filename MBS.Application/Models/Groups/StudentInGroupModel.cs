using MBS.Core.Entities;

public class GroupStudentsResponseDTO
{
    public Project Project { get; set; }
    public List<StudentInGroupDTO> Students { get; set; }
}

public class StudentInGroupDTO
{
    public string StudentId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public Major Major { get; set; }
    public string University { get; set; }

    public int WalletPoint {  get; set; }



}
