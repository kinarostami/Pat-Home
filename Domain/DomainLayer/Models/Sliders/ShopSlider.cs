using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Sliders;

public class ShopSlider : BaseEntity
{
    public bool IsActive { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string ImageName { get; set; }
    public string Description { get; set; }
}
