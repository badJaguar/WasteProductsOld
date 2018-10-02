import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';
import { Group } from 'src/app/models/groups/group';
import { GroupBoard } from 'src/app/models/groups/groupBoard';
import { GroupProduct } from 'src/app/models/groups/groupProduct';
import { GroupComment } from 'src/app/models/groups/groupComment';
import { GroupUser } from 'src/app/models/groups/groupUser';

import { environment } from '../../../environments/environment.prod';
import { Observable } from 'rxjs';
import { GroupOfUser } from 'src/app/models/users/group-of-user';

@Injectable({
  providedIn: 'root'
})
export class GroupService extends BaseHttpService{

  private apiUrl = `${environment.apiHostUrl}/api/groups`;

  constructor(httpService: HttpClient) {
    super(httpService, loggingService);
  }
  ///group
  createGroup(item:Group):Observable<Group>{
    return this.http.post<Group>(`${this.apiUrl}`, item);
  }

  updateGroup(item:Group):Observable<Group>{
    return this.http.put<Group>(`${this.apiUrl}/${item.Id}`, item);
  }

  deleteGroup(item:Group):Observable<any>{
    return this.http.delete<any>(`${this.apiUrl}/${item.Id}/${item.AdminId}`);
  }
  ///board
  createGroupBoard(item:GroupBoard):Observable<GroupBoard>{
    return this.http.post<GroupBoard>(`${this.apiUrl}/${item.GroupId}/board`, item);
  }

  updateGroupBoard(item:GroupBoard):Observable<GroupBoard>{
    return this.http.put<GroupBoard>(`${this.apiUrl}/${item.GroupId}/board`, item);
  }

  deleteGroupBoard(item:GroupBoard):Observable<any>{
    return this.http.delete<any>(`${this.apiUrl}/${item.GroupId}/board/${item.CreatorId}`);
  }
  ///product
  createGroupProduct(item:GroupProduct, groupId:string, userId:string):Observable<GroupProduct>{
    return this.http.post<GroupProduct>(`${this.apiUrl}/${groupId}/product/${userId}`, item);
  }

  updateGroupProduct(item:GroupProduct, groupId:string, userId:string):Observable<GroupProduct>{
    return this.http.put<GroupProduct>(`${this.apiUrl}/${groupId}/product/${userId}`, item);
  }

  deleteGroupProduct(item:GroupProduct, groupId:string, userId:string):Observable<any>{
    return this.http.delete<any>(`${this.apiUrl}/${groupId}/product/${item.GroupBoardId}/${item.Id}/${userId}`);
  }
  ///comment
  createGroupComment(item:GroupComment, groupId:string):Observable<GroupComment>{
    return this.http.post<GroupComment>(`${this.apiUrl}/${groupId}/comment`, item);
  }

  updateGroupComment(item:GroupComment, groupId:string):Observable<GroupComment>{
    return this.http.put<GroupComment>(`${this.apiUrl}/${groupId}/comment`, item);
  }

  deleteGroupComment(item:GroupComment, groupId:string):Observable<any>{
    return this.http.delete<any>(`${this.apiUrl}/${groupId}/comment/${item.GroupBoardId}/${item.Id}/${item.CommentatorId}`);
  }
  ///user
  inviteGroupUser(item:GroupUser, groupId:string, adminId:string):Observable<any>{
    return this.http.post<any>(`${this.apiUrl}/${groupId}/invite/${adminId}`, item);
  }
  kickGroupUser(item:GroupUser, groupId:string, adminId:string):Observable<any>{
    return this.http.post<any>(`${this.apiUrl}/${groupId}/kick/${adminId}`, item);
  }
  giveRightToCreateBoardsGroupUser(item:GroupUser, groupId:string, adminId:string):Observable<GroupUser>{
    return this.http.put<GroupUser>(`${this.apiUrl}/${groupId}/giveright/${adminId}`, item);
  }
  takeAwayRightToCreateBoardsGroupUser(item:GroupUser, groupId:string, adminId:string):Observable<GroupUser>{
    return this.http.put<GroupUser>(`${this.apiUrl}/${groupId}/takeawayright/${adminId}`, item);
  }

  getGroupsOfUserUrl = `http://localhost:2189/api/user/0/groups`;

  loadGroupsOfUser():Observable<GroupOfUser[]> {
    return this.http.get<GroupOfUser[]>(this.getGroupsOfUserUrl);
   }
}
