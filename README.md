# Reactivities

Reactivities is a social app for making activities and meeting new people.

Built with .Net 6 and React 17

[Demo link](https://dotnetreactivities.herokuapp.com)

[Security Headers Rating](https://securityheaders.com/?q=https%3A%2F%2Fdotnetreactivities.herokuapp.com&followRedirects=on)

<br/>


## Features

* Login or register to app
* Login with Facebook (pending app approval from facebook may not work at the moment)
* Filter activities by user going, user hosting or/and date 
* Create activitiy with 6 categories or join any of them
* Host can edit or cancel activity
* Infinity Scroll
* Follower feature
* Add or change your username and bio
* See followers, followings and activities of user
* Add photos and change your profile photo (Using Cloudinary Api)
* Real time comments on activities (using SignalR)
* Secure logins (Jwt tokens and Identity libaray)
* Exception Middleware for catching exceptions

## ScreenShots

### Login Page

- Users can login or register to app from this page 

![resim](https://user-images.githubusercontent.com/78491395/165123969-4258a4f1-647c-40c6-8d94-f0867e3124f6.png)

- Login

![resim](https://user-images.githubusercontent.com/78491395/165124025-e007c5bd-3587-4e17-85a3-3cfdd260d041.png)

- Login Validation

![resim](https://user-images.githubusercontent.com/78491395/165124459-b2b91f08-ead2-42d7-a5a0-42e43295b39e.png)

- Register

![resim](https://user-images.githubusercontent.com/78491395/165124161-36668a75-1b0e-4fa1-8642-9b3253a7073f.png)

- Register Validation

![resim](https://user-images.githubusercontent.com/78491395/165124308-1866150a-ea34-46d9-be56-d579604d6534.png)

### Homepage

- Users can see the all activities or filter them by date or by going/hosting 

![resim](https://user-images.githubusercontent.com/78491395/165124676-7a97ace1-389e-4459-b695-6a47e891630a.png)

### Activity Pages

- Users can attend or host activity 
- Users can cancel or re-activate the activity, manage event details or enter a comment

- Activity page (hosting event)

![resim](https://user-images.githubusercontent.com/78491395/165124796-302d720f-aa5b-42c5-a03e-9b342dcbc2cd.png)

- Cancelled activity (hosting event)

![resim](https://user-images.githubusercontent.com/78491395/165125701-644ca506-f664-4dc9-8810-032fd2e79d1f.png)

- Edit activity (hosting event)

![resim](https://user-images.githubusercontent.com/78491395/165125775-646ad17a-10ba-4b87-a176-770d21b3324a.png)


- Activity page (attending)

![resim](https://user-images.githubusercontent.com/78491395/165124934-8586bc30-1e3b-4432-9649-096c93e28114.png)

- Activity page (not attending)

![resim](https://user-images.githubusercontent.com/78491395/165125992-1f02f58a-f8d2-4fe2-8f63-cd1bfd4edbec.png)

- Create activity page

![resim](https://user-images.githubusercontent.com/78491395/165126065-d2ef1089-c52f-459a-bcdd-0b965b106630.png)

- Create activity validation

![resim](https://user-images.githubusercontent.com/78491395/165126130-dd5c0b42-ea49-4dcd-9944-1397fdbd8b8e.png)

### User Profile Page

- Users can edit their bio, manage their profile photos, see their events and see their followers/following

![resim](https://user-images.githubusercontent.com/78491395/165126410-7866b45d-e663-4b28-a575-dc749e06b96e.png)

- Add profile photo

![resim](https://user-images.githubusercontent.com/78491395/165126572-4573f191-8785-4f24-9e75-f64d2437948e.png)

- Followings

![resim](https://user-images.githubusercontent.com/78491395/165126774-2cbe6e80-f63d-4387-aba7-285065c95472.png)

- Events

![resim](https://user-images.githubusercontent.com/78491395/165126850-9faca9e6-3070-4883-b8aa-a9c24ede77fd.png)
