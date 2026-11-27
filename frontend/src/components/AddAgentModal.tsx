import { useState } from "react";
import { addAgent } from "../services/agentService";
import styles from "../pages/DashboardPage.module.css";

type AddAgentModalProps = {
  onClose: () => void;
  onAdded: () => void;
};

function AddAgentModal({ onClose, onAdded }: AddAgentModalProps) {
  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [companyName, setCompanyName] = useState("");
  const [formError, setFormError] = useState("");

  async function handleAddAgent() {
    setFormError("");

    try {
      await addAgent({
        email: email,
        agentFirstName: firstName,
        agentLastName: lastName,
        phoneNumber: phoneNumber || undefined,
        companyName: companyName || undefined,
      });

      onAdded();
    } catch {
      setFormError("Could not add agent.");
    }
  }

  return (
    <div className={styles.modalOverlay}>
      <form
        className={styles.modal}
        onSubmit={(e) => {
          e.preventDefault();
          handleAddAgent();
        }}
      >
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
          <button type="button" onClick={onClose}>
            Cancel
          </button>

          <button type="submit">Add Agent</button>
        </div>
      </form>
    </div>
  );
}

export default AddAgentModal;
