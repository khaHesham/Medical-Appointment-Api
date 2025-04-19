# Medical Appointment API

This project is a RESTful API for managing medical appointments. It allows users to create, update, and manage appointments, patients, and doctors. The system is designed to provide a seamless experience for managing medical appointments.

## Features
- Manage patients, doctors, and appointments.
- Handle scheduling conflicts.
- CRUD operations for all entities.
- Secure and scalable architecture.

## System Design

### Entities
1. **User** (Base Entity):
    - `id` (int): Unique identifier for the user.
    - `first_name` (string): Fisrt name of the user.
    - `last_name` (string): Last name of the user.
    - `email` (string): Contact email of the user.
    - `phone_number` (string): Contact phone number.

1. **Patient** (Inherits from User):
    - `date_of_birth` (date): Patient's date of birth.

2. **Doctor** (Inherits from User):
    - `specialization` (string): Doctor's area of expertise.

3. **Appointment**:
    - `id` (int): Unique identifier for the appointment.
    - `patient_id` (int): Reference to the patient.
    - `doctor_id` (int): Reference to the doctor.
    - `appointment_date` (datetime): Date and time of the appointment.
    - `status` (string): Status of the appointment (e.g., scheduled, completed, canceled).
    - `reason` (string): The appointment reason.

### Architecture
The system follows a layered architecture:
1. **Controller Layer**: Handles HTTP requests and responses.
2. **Service Layer**: Contains business logic.
3. **Repository Layer**: Manages database interactions using the Unit of Work pattern and generic repositories.
4. **Database**: Stores all data entities.

### Database
- The application uses SQL Server as the database.
- Entity Framework Core (EF Core) is used with a code-first approach for database modeling and migrations.

## How to Run

### Prerequisites
- Docker and Docker Compose installed.
- `.env` file placed in the root folder of the project with necessary environment variables.

### Steps
1. Clone the repository:
    ```bash
    git clone https://github.com/khaHesham/Medical-Appointment-Api.git
    cd Medical-Appointment-Api
    ```

2. Ensure the `.env` file is in the root directory with the required variables:
    ```
    DATABASE_URL=your_database_url
    JWT_SECRET=your_jwt_secret
    ....
    ....(rest of env variables)
    ```

3. Run the application using Docker Compose:
    ```bash
    docker-compose up --build -d
    ```

4. Access the API at `http://0.0.0.0:5172`

### Stopping the Application
To stop the application, run:
```bash
docker-compose down
```

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.
