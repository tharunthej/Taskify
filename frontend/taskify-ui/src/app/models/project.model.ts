import { TaskItem } from "./task.model";

export interface Project {
    id: number;
    name: string;
    description?: string;
    createdBy: number;      // User ID
    createdAt: Date;
    
    // Optional nested objects
    creator?: {
      id: number;
      username: string | null;
      email?: string;
    };
    
    members?: ProjectMember[];
    tasks?: TaskItem[];
  }
  
  export interface ProjectMember {
    userId: number;
    projectId: number;
    roleId: number;        // e.g., 1 = Admin, 2 = Member
    user?: {
      id: number;
      username: string;
    };
  }