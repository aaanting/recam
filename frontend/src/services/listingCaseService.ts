import type { ListingCase } from "../types/listingCase";
import { checkToken } from "./authService";

export type CreateListingCaseRequest = {
  title: string;
  description: string;
  street: string;
  city: string;
  state: string;
  postcode: number;
  longitude: number;
  latitude: number;
  price: number;
  bedrooms: number;
  bathrooms: number;
  garages: number;
  floorArea: number;
  propertyType: string;
  saleCategory: string;
};

export async function getListingCases(): Promise<ListingCase[]> {
  const token = checkToken();

  const response = await fetch("/api/ListingCase", {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch listing cases.");
  }

  return await response.json();
}

export async function createListingCase(listingCase: CreateListingCaseRequest) {
  const token = checkToken();

  const response = await fetch("/api/ListingCase", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(listingCase),
  });

  if (!response.ok) {
    throw new Error("Failed to create listing case.");
  }
}
