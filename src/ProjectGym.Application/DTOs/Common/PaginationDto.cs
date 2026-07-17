using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Application.DTOs.Common;

public class PaginationDto
{
    private int _pageSize = 10;

    [Range(1,int.MaxValue, ErrorMessage ="Sayfa numarası en az 1 olmalıdır.")]
    public int PageNumber { get; set; }=1;

    public int PageSize
    {
        get=>_pageSize;
        set=>_pageSize=value>100 ? 100 : value;
    }
}
                      