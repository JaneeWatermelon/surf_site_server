import { Observable, of } from "rxjs";
import { UserData } from "../models/user_data";
import { LoginData } from "../models/login_data";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class LoginApiService {

    private users: UserData[] = [
        new UserData(
            1,
            "Jane",
            "",
            "12345",
            "assets/images/default_avatar.jpg",
            "",
            "",
            "",
            "",
            "",
            "13.06.2019",
            "",
        ),
    ];

    login(data: LoginData): Observable<boolean> {
        // return this.http.post<boolean>('/api/login', data);

        const user = this.users.find(u =>
            (u.login === data.nickname_or_email ||
             u.email === data.nickname_or_email) &&
            u.password === data.password
        );

        return of(!!user);
    }

    getUsers(): Observable<UserData[]> {
        return of([...this.users]);
    }
}