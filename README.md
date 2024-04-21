# One-Time Password (OTP) Generation System

The **One-Time Password (OTP) Generation System** provides a secure and user-friendly solution for generating OTPs in a banking application. It prioritizes security and data protection while delivering a seamless authentication experience.

## Project Overview

- **Secure OTP Generation**: Randomly generated OTPs with encryption ensure confidentiality and secure transmission.
- **Time-bound OTPs**: OTPs are valid for a limited period, enhancing security. The validity period is customizable.
- **User-Friendly Interface**: A clear and intuitive interface makes entering the OTP simple and straightforward.
- **Effective Error Handling**: Clear messaging for any issues keeps the process smooth.
- **Toast Message Notifications**: Users receive OTPs through toast messages, which stay visible while the OTP is valid.

## Backend Implementation

- **Framework**: Developed using ASP.NET Core.
- **Database**: Utilizes SQL Server for data persistence.
- **Features**:
    - **User Registration**: Allows users to register to the system.
    - **User Login**: Enables login with a username and password.
    - **OTP Generation**: Generates a one-time password (OTP) for logged-in users.
    - **OTP Login**: Supports logging in with an OTP for logged-in users.

## Frontend Implementation

- **Framework**: Built with React and Next.js.
- **Pages**:
    - **Login**: Allows users to log in with their credentials.
    - **Register**: Provides a registration page for new users.
    - **Login with OTP**: Allows logged-in users to log in using an OTP.
    - **Logged-in Users Page**: A dedicated page for logged-in users.
- **Authorization**:
    - **Access Control**: Users who are not logged in cannot access the "Login with OTP" page or the page for logged-in users.
    - **OTP Restriction**: Users who have not logged in using an OTP cannot access the page for logged-in users.
- **Authentication**: Utilizes JWT tokens for login, stored in cookies for a limited time.

## How to Test

The application can currently be tested exclusively via the React application due to the need for encrypted passwords in the body of login/register endpoints, which require a secret key known only to the server and client.

### Testing Steps:

1. **Open the Web Client**: Access the React application. ![image](https://github.com/leoivv39/OTP-System/assets/118171860/b1b75804-927d-4001-947e-5c27059185b1)

2. **Register**: Register a new account in the application. ![image](https://github.com/leoivv39/OTP-System/assets/118171860/b94058bb-822b-4478-8aef-8c831ec89b6b)

3. **Login**: Log in using the newly created account. ![image](https://github.com/leoivv39/OTP-System/assets/118171860/f45db211-cdc4-466f-9f72-2c290dbf87aa)

4. **Enter OTP**: Input the one-time password (OTP) received via toast message. ![image](https://github.com/leoivv39/OTP-System/assets/118171860/701eee25-975f-4730-817d-5bc89962ce26)

5. **Access the Application**: Once the OTP is verified, you will be logged in to the application. ![image](https://github.com/leoivv39/OTP-System/assets/118171860/55d9602e-c5e4-4f69-a87f-5dba7faae702)

