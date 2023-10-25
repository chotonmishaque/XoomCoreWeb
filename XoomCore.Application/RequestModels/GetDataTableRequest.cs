namespace XoomCore.Application.RequestModels;

public class GetDataTableRequest
{
    public int StartFrom { get; set; } = 1;
    public int NoOfRecordsToFetch { get; set; } = 10;
    public int Limit { get; set; } = 10;
    public int PageNo { get; set; } = 1;
    public string SearchText { get; set; } = "";
}
