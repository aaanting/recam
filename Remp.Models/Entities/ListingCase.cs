using System;
using Remp.Models.Enums;

namespace Remp.Models.Entities;

public class ListingCase
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Street { get; set; }
	public string City { get; set; }
	public string State { get; set; }
	public int Postcode { get; set; }
	public decimal Longitude { get; set; }
	public decimal Latitude { get; set; }
	public double Price { get; set; }
	public int Bedrooms { get; set; }
	public int Bathrooms { get; set; }
	public int Garages { get; set; }
	public double FloorArea { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.Now;
	public bool IsDeleted { get; set; } = false;
	public PropertyType PropertyType { get; set; }
	public SaleCategory SaleCategory { get; set; }
	public ListcaseStatus ListcaseStatus { get; set; } = Models.Enums.ListcaseStatus.Created;

	// listingcase -- User N:1
	public string UserId { get; set; }
	public User User { get; set; }

	// listingcase -- CaseContact 1:N
	public List<CaseContact> CaseContacts { get; set; }

	// listingcase -- MediaAsset 1:N
	public List<MediaAsset> MediaAssets { get; set; }

	//agenct -- listingcase N:N
	public List<Agent> Agents { get; set; }
}
