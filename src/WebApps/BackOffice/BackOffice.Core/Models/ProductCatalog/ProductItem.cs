using System.ComponentModel.DataAnnotations;

namespace BackOffice.Core.Models.ProductCatalog;

public class ProductItem : EntityModelBase
{
    public Guid? ItemTypeId { get; set; }

    public Guid? ParentId { get; set; }

    public ProductType? ItemType { get; set; }

    public Guid? CatalogBrandId { get; set; }

    public ProductBrand? Brand { get; set; }

    [Required(ErrorMessage = "The Description field is required")]
    public string Description { get; set; } = null!;

    [MaxLength(13)]
    public string? Barcode { get; set; }

    // decimal(18,2)
    //[RegularExpression(@"^\d+(\.\d{0,2})*$", ErrorMessage = "The field Price must be a positive number with maximum two decimals.")]
    [Range(0.01, 100000)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public string PictureUri { get; set; } = string.Empty;

    public string PictureBase64 { get; set; } = string.Empty;

    public string PictureName { get; set; } = null!;

    private const int ImageMaximumBytes = 512000;

    public static string? IsValidImage(string pictureName, string pictureBase64)
    {
        if (string.IsNullOrEmpty(pictureBase64))
        {
            return "File not found!";
        }
        var fileData = Convert.FromBase64String(pictureBase64);

        if (fileData.Length <= 0)
        {
            return "File length is 0!";
        }

        if (fileData.Length > ImageMaximumBytes)
        {
            return "Maximum length is 512KB";
        }

        if (!IsExtensionValid(pictureName))
        {
            return "File is not image";
        }

        return null;
    }

    private static bool IsExtensionValid(string fileName)
    {
        var extension = Path.GetExtension(fileName);

        return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
    }
}
