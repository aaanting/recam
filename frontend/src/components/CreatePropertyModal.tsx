import { useState } from "react";
import { createListingCase } from "../services/listingCaseService";
import styles from "../pages/DashboardPage.module.css";

const saleCategoryOptions = [
  { value: "ForSale", label: "For Sale" },
  { value: "Auction", label: "Auction" },
  { value: "ForRent", label: "For Rent" },
];

const propertyTypeOptions = [
  { value: "House", label: "House" },
  { value: "Townhouse", label: "Townhouse" },
  { value: "Villa", label: "Villa" },
  { value: "Unit", label: "Apartment / Unit" },
  { value: "Others", label: "Others" },
];

type CreatePropertyModalProps = {
  onClose: () => void;
  onCreated: () => void;
};

function CreatePropertyModal({ onClose, onCreated }: CreatePropertyModalProps) {
  const [title, setTitle] = useState("");
  const [saleCategory, setSaleCategory] = useState("ForSale");
  const [propertyType, setPropertyType] = useState("House");
  const [street, setStreet] = useState("");
  const [city, setCity] = useState("");
  const [state, setState] = useState("");
  const [postcode, setPostcode] = useState("");
  const [bedrooms, setBedrooms] = useState("");
  const [bathrooms, setBathrooms] = useState("");
  const [garages, setGarages] = useState("");
  const [floorArea, setFloorArea] = useState("");
  const [formError, setFormError] = useState("");

  async function handleCreateProperty() {
    setFormError("");

    try {
      await createListingCase({
        title: title,
        description: title,
        street: street,
        city: city,
        state: state,
        postcode: Number(postcode),
        longitude: 0,
        latitude: 0,
        price: 0,
        bedrooms: Number(bedrooms),
        bathrooms: Number(bathrooms),
        garages: Number(garages),
        floorArea: Number(floorArea),
        propertyType: propertyType,
        saleCategory: saleCategory,
      });

      onCreated();
    } catch {
      setFormError("Could not create property.");
    }
  }

  return (
    <div className={styles.modalOverlay}>
      <form
        className={styles.modal}
        onSubmit={(e) => {
          e.preventDefault();
          handleCreateProperty();
        }}
      >
        <h2>Create Property</h2>

        <label>
          Property Title
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </label>

        <div>
          <p>Property Status</p>
          {saleCategoryOptions.map((option) => (
            <label key={option.value}>
              <input
                type="radio"
                name="saleCategory"
                value={option.value}
                checked={saleCategory === option.value}
                onChange={(e) => setSaleCategory(e.target.value)}
              />
              {option.label}
            </label>
          ))}
        </div>

        <div>
          <p>Property Type</p>
          {propertyTypeOptions.map((option) => (
            <label key={option.value}>
              <input
                type="radio"
                name="propertyType"
                value={option.value}
                checked={propertyType === option.value}
                onChange={(e) => setPropertyType(e.target.value)}
              />
              {option.label}
            </label>
          ))}
        </div>

        <label>
          Street
          <input
            type="text"
            value={street}
            onChange={(e) => setStreet(e.target.value)}
            required
          />
        </label>

        <label>
          City
          <input
            type="text"
            value={city}
            onChange={(e) => setCity(e.target.value)}
            required
          />
        </label>

        <label>
          State
          <input
            type="text"
            value={state}
            onChange={(e) => setState(e.target.value)}
            required
          />
        </label>

        <label>
          Postcode
          <input
            type="number"
            value={postcode}
            onChange={(e) => setPostcode(e.target.value)}
            required
          />
        </label>

        <label>
          Bed
          <input
            type="number"
            min="0"
            value={bedrooms}
            onChange={(e) => setBedrooms(e.target.value)}
            required
          />
        </label>

        <label>
          Bath
          <input
            type="number"
            min="0"
            value={bathrooms}
            onChange={(e) => setBathrooms(e.target.value)}
            required
          />
        </label>

        <label>
          Car
          <input
            type="number"
            min="0"
            value={garages}
            onChange={(e) => setGarages(e.target.value)}
            required
          />
        </label>

        <label>
          Area (m²)
          <input
            type="number"
            min="0"
            value={floorArea}
            onChange={(e) => setFloorArea(e.target.value)}
            required
          />
        </label>

        {formError && <p>{formError}</p>}

        <div className={styles.modalButtons}>
          <button type="button" onClick={onClose}>
            Cancel
          </button>

          <button type="submit">Save</button>
        </div>
      </form>
    </div>
  );
}

export default CreatePropertyModal;
