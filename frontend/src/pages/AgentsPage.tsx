import { type FormEvent, useEffect, useState } from "react";
import { addAgent, getCompanyAgents } from "../services/agentService";
import searchIcon from "../assets/search.svg";
import styles from "./DashboardPage.module.css";

function AgentsPage() {
  const [agents, setAgents] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [showModal, setShowModal] = useState(false);

  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [companyName, setCompanyName] = useState("");
  const [formError, setFormError] = useState("");

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

  // Add a new agent
  async function handleAddAgent(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setFormError("");

    try {
      await addAgent({
        email: email,
        agentFirstName: firstName,
        agentLastName: lastName,
        phoneNumber: phoneNumber || undefined,
        companyName: companyName || undefined,
      });

      const data = await getCompanyAgents();
      setAgents(data);

      setShowModal(false);
      setEmail("");
      setFirstName("");
      setLastName("");
      setPhoneNumber("");
      setCompanyName("");

    } catch {
      setFormError("Could not add agent.");
    }
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
        <div className={styles.modalOverlay}>
          <form className={styles.modal} onSubmit={handleAddAgent}>
            <h2>Add New Agent</h2>

            <label>
              First Name
              <input
                type="text"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                required
              />
            </label>

            <label>
              Last Name
              <input
                type="text"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                required
              />
            </label>

            <label>
              Email
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </label>

            <label>
              Company Name
              <input
                type="text"
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
              />
            </label>

            <label>
              Phone Number
              <input
                type="tel"
                value={phoneNumber}
                onChange={(e) => setPhoneNumber(e.target.value)}
              />
            </label>

            {formError && <p>{formError}</p>}

            <div className={styles.modalButtons}>
              <button
                type="button"
                onClick={() => setShowModal(false)}
              >
                Cancel
              </button>

              <button
                type="submit"
              >
                Add Agent
              </button>
            </div>
          </form>
        </div>
      )}
    </main>
  );
}

export default AgentsPage;
