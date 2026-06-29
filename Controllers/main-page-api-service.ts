import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { Post } from "../models/post";

@Injectable()
export class MainPageApiService {

    constructor(private httpClient: HttpClient) {

    }

    getPosts(): Observable<Post[]> {

        return this.httpClient.get<any[]>("http://localhost:5024/api/GetPosts");

        return of([
            {id: 1, title: "post1", body: "post1"},
            {id: 2, title: "post2", body: "post1"}
        ]);
    }

    savePost(post: any): Observable<void> {
        //return this.httpClient.post<void>("/api/SavePost", post);
        return of(undefined);
    }
}