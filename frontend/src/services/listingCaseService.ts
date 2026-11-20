import type { ListingCase } from "../types/listingCase";

export async function getListingCases(): Promise<ListingCase[]> {
  const response = await fetch("/api/ListingCase");

  if (!response.ok) {
    throw new Error("Failed to fetch listing cases.");
  }

  return await response.json();
}
