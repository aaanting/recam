import type { Agent } from "../types/agent";

export async function getCompanyAgents(): Promise<Agent[]> {
  const token = sessionStorage.getItem("token");

  if (!token) {
    throw new Error("Please sign in to view agents.");
  }

  const response = await fetch("/api/Agent/company-agents", {
    headers: { Authorization: `Bearer ${token}` },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch agents.");
  }

  return await response.json();
}
