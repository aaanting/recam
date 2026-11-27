import { useEffect, useState } from "react";
import { getCompanyAgents } from "../services/agentService";
import type { Agent } from "../types/agent";
import AddAgentModal from "../components/AddAgentModal";
import searchIcon from "../assets/search.svg";
import styles from "./DashboardPage.module.css";

function AgentsPage() {
  const [agents, setAgents] = useState<Agent[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    async function fetchAgents() {
      try {
        const data = await getCompanyAgents();
        setAgents(data);
      } catch {
        setError("Could not load agents.");
      } finally {
        setLoading(false);
      }
    }

    fetchAgents();
  }, []);

  async function handleAdded() {
    setShowModal(false);
    const data = await getCompanyAgents();
    setAgents(data);
  }

  return (
    <main className={styles.page}>
      <h1 className={styles.title}>Hi, Welcome!</h1>

      <div className={styles.toolbar}>
        <label className={styles.searchBar}>
          <img src={searchIcon} alt="" width={20} height={20} />

          <input
            className={styles.search}
            type="search"
            placeholder="Search agents..."
          />
        </label>

        <button
          className={styles.primaryButton}
          type="button"
          onClick={() => setShowModal(true)}
        >
          + Add New Agent
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th scope="col">Agent Name</th>
              <th scope="col">Company</th>
              <th scope="col">Number</th>
              <th scope="col">Email</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>

          <tbody>
            {loading && (
              <tr>
                <td colSpan={5}>Loading agents...</td>
              </tr>
            )}

            {error && (
              <tr>
                <td colSpan={5}>{error}</td>
              </tr>
            )}

            {!loading && !error && agents.length === 0 && (
              <tr>
                <td colSpan={5}>No agents found</td>
              </tr>
            )}

            {agents.map((agent) => (
              <tr key={agent.id}>
                <td>
                  {agent.agentFirstName} {agent.agentLastName}
                </td>
                <td>{agent.companyName || "—"}</td>
                <td>{agent.phoneNumber || "—"}</td>
                <td>{agent.email || "—"}</td>
                <td>…</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showModal && (
        <AddAgentModal
          onClose={() => setShowModal(false)}
          onAdded={handleAdded}
        />
      )}
    </main>
  );
}

export default AgentsPage;
