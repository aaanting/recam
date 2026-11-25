import type { Agent } from "../types/agent";
import { checkToken } from "./authService";

export type CreateAgentRequest = {
  email: string;
  agentFirstName: string;
  agentLastName: string;
  phoneNumber?: string;
  companyName?: string;
  avatarUrl?: string;
};

export async function getCompanyAgents(): Promise<Agent[]> {
  const token = checkToken();

  const response = await fetch("/api/Agent/company-agents", {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch agents.");
  }

  const data = await response.json();

  return data;
}

export async function addAgent(agent: CreateAgentRequest) {
  const token = checkToken();

  const response = await fetch("/api/Agent", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(agent),
  });

  if (!response.ok) {
    throw new Error("Failed to add agent.");
  }
}
