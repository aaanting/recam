import { useEffect, useState } from "react";
import searchIcon from "../assets/search.svg";
import styles from "./DashboardPage.module.css";

import type { ListingCase } from "../types/listingCase";

function DashboardPage() {
  const [listings, setListings] = useState<ListingCase[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function fetchListings() {
      try {
        const response = await fetch("/api/ListingCase");

        if (!response.ok) {
          throw new Error("Failed to fetch listings");
        }

        const data = await response.json();
        setListings(data);
      } catch (error) {
        console.error(error);
        setError("Could not load listing cases.");
      } finally {
        setLoading(false);
      }
    }

    fetchListings();
  }, []);

  function formatDate(date: string) {
    const parts = date.slice(0, 10).split("-");
    return `${parts[2]}/${parts[1]}/${parts[0]}`;
  }

  function formatAddress(listing: ListingCase) {
    return `${listing.street}, ${listing.city}, ${listing.state}, ${listing.postcode}`;
  }

  return (
    <main className={styles.dashboard}>
      <h1 className={styles.title}>Hi, welcome User</h1>

      <div className={styles.toolbar}>
        <label className={styles.searchBar}>
          <img src={searchIcon} alt="" width={20} height={20} />

          <input
            className={styles.search}
            type="search"
            aria-label="Search listing cases"
            placeholder="Search from listing case"
          />
        </label>

        <button className={styles.createOrder} type="button">
          + Create Property
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.orders} aria-label="Listing cases">
          <thead>
            <tr>
              <th>Property #</th>
              <th>Property Type</th>
              <th>Property Address</th>
              <th>Created At</th>
              <th>Status</th>
              <th className={styles.actions}>Actions</th>
            </tr>
          </thead>

          <tbody>
            {/* Show loading message */}
            {loading && (
              <tr>
                <td colSpan={6}>Loading listing cases...</td>
              </tr>
            )}

            {!loading && error && (
              <tr>
                <td colSpan={6}>{error}</td>
              </tr>
            )}

            {!loading && !error && listings.length === 0 && (
              <tr>
                <td colSpan={6}>No listing cases found.</td>
              </tr>
            )}

            {!loading &&
              !error &&
              listings.map((listing) => (
                <tr key={listing.id}>
                  <td>{String(listing.id).padStart(6, "0")}</td>
                  <td>{listing.propertyType}</td>
                  <td>{formatAddress(listing)}</td>
                  <td>{formatDate(listing.createdAt)}</td>
                  <td>{listing.listcaseStatus}</td>
                  <td className={styles.actions}>…</td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </main>
  );
}

export default DashboardPage;
