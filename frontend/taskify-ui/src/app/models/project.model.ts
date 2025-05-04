import { TaskItem } from "./task.model";

export interface Project {
  id: number;
  name: string;
  description?: string;
  createdBy: number;      // User ID of creator
  createdAt: Date;
  updatedAt?: Date;
  
  // Required nested objects (matches backend navigation properties)
  creator: {
    id: number;
    username: string;
    email: string;
  };
  
  members?: ProjectMember[];
  tasks?: TaskItem[];
}

export interface ProjectMember {
  id: number;
  projectId: number;
  userId: number;
  userRoleId: number;    // 1 = Admin, 2 = Member
  user: {
    id: number;
    username: string;
    email: string;
  };
  role?: {
    id: number;
    name: string;        // e.g., "Admin", "Member"
  };
}

// Optional: If you need a simplified version for listings
export interface ProjectSummary {
  id: number;
  name: string;
  description?: string;
  creatorName: string;
  memberCount: number;
  taskCount: number;
  createdAt: Date;
}

// Response DTO interface
export interface ProjectResponse {
  id: number;
  name: string;
  description?: string;
  creatorName: string;
  creatorId: number;
  createdAt: Date;
  memberCount: number;
  taskCount: number;
  members?: ProjectMemberResponse[];
  tasks?: TaskBrief[];
}

export interface ProjectMemberResponse {
  userId: number;
  username: string;
  role: string;
  email: string;
}

export interface TaskBrief {
  id: number;
  title: string;
  status: string;
}