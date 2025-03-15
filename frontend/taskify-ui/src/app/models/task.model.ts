// src/app/models/task.model.ts

export interface TaskItem {
    id: number;
    title: string;
    description?: string;
    statusId: number;       // Foreign key to TaskStatus (e.g., 1 = "To Do")
    priorityId: number;     // Foreign key to TaskPriority (e.g., 1 = "Low")
    projectId: number;      // Foreign key to Project
    assignedTo?: number;    // Foreign key to User (nullable)
    createdBy: number;      // Foreign key to User (creator)
    createdAt: Date;
    updatedAt?: Date;
  
    // Optional nested objects (if your API returns expanded data)
    project?: {
      id: number;
      name: string;
    };
    assignee?: {
      id: number;
      username: string;
    };
    creator?: {
      id: number;
      username: string;
    };
    comments?: Comment[];
    attachments?: Attachment[];
  }
  
  // Optional: Define related interfaces if needed
  export interface Comment {
    id: number;
    content: string;
    userId: number;
    createdAt: Date;
    user?: {
      id: number;
      username: string;
    };
  }
  
  export interface Attachment {
    id: number;
    fileName: string;
    fileUrl: string;
    uploadedBy: number;
    uploadedAt: Date;
    user?: {
      id: number;
      username: string;
    };
  }