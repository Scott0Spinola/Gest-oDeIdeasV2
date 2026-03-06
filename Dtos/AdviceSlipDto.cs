using System;

namespace GestãoDeIdeasV2.Dtos;

public class AdviceSlipDto
{
    public AdviceSlip? Slip { get; set; }
}

public class AdviceSlip
{
    public int Id { get; set; }
    public string? Advice { get; set; }
}

