import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from '../../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = 'http://localhost:5089/api/projects';

  constructor(private http: HttpClient) { }

  // Get all projects for current user
  getAllProjects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.apiUrl);
  }

  // Get single project by ID
  getProjectById(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/${id}`);
  }

  // Create new project
  createProject(projectData: { name: string; description?: string }): Observable<Project> {
    return this.http.post<Project>(this.apiUrl, projectData);
  }

  // Update existing project
  updateProject(id: number, projectData: { name?: string; description?: string }): Observable<Project> {
    return this.http.put<Project>(`${this.apiUrl}/${id}`, projectData);
  }

  // Delete project
  deleteProject(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Add member to project
  addMember(projectId: number, userId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${projectId}/members/${userId}`, {});
  }
}