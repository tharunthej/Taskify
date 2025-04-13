// user.model.ts
export interface User {
    id: number;
    username: string;
    email: string;
    createdAt: Date;
    // Add these if you need them in the frontend
    projectMemberships?: ProjectMember[]; 
    assignedTasks?: TaskItem[];
  }
  
  // Optional: If you need a separate interface for auth responses
  export interface AuthUser extends User {
    token?: string;  // Only for login/registration responses
  }
  
  // Helper types for nested relationships
  interface ProjectMember {
    projectId: number;
    // Add other relevant properties
  }
  
  interface TaskItem {
    id: number;
    title: string;
    // Add other relevant properties
  }