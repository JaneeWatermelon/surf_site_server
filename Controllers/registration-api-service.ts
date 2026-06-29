import { Observable, of } from "rxjs";
import { UserData } from "../models/user_data";
import { RegistrationData } from "../models/registration_data";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class RegistrationApiService {

    private users: UserData[] = [];

    register(data: RegistrationData): Observable<void> {
        // return this.http.post<void>('/api/register', data);

        const avatar = data.photo
            ? URL.createObjectURL(data.photo)
            : 'assets/images/default_avatar.jpg';

        const user = new UserData(
            this.users.length + 1,
            data.nickname,
            data.email,
            data.password,
            avatar,
            data.last_name,
            data.first_name,
            data.contacts,
            data.about,
            data.achievements,
            new Date().toLocaleString(),
            new Date().toLocaleString()
        );

        this.users.push(user);

        return of(void 0);
    }

    getUsers(): Observable<UserData[]> {
        return of([...this.users]);
    }
}