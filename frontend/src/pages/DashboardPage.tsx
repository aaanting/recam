import searchIcon from "../assets/search.svg";
import styles from "./DashboardPage.module.css";

function DashboardPage() {
  return (
    <main className={styles.dashboard}>
      <h1 className={styles.title}>Hi, welcome User</h1>

      <div className={styles.toolbar}>
        <label className={styles.searchBar}>
          <img src={searchIcon} alt="" width={20} height={20} />
          <input
            className={styles.search}
            type="search"
            aria-label="Search orders"
            placeholder="Search from order list"
          />
        </label>
        <button className={styles.createOrder} type="button">
          + Create Order
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.orders} aria-label="Orders">
          <thead>
            <tr>
              <th scope="col">Order Number</th>
              <th scope="col">Client Name</th>
              <th scope="col">Property Address</th>
              <th scope="col">Order Time</th>
              <th scope="col">Status</th>
              <th scope="col" aria-label="Actions" className={styles.actions}>…</th>
            </tr>
          </thead>
          <tbody />
        </table>
      </div>
    </main>
  );
}

export default DashboardPage;
