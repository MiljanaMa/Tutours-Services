INSERT INTO users (id, username, password, role, is_active, email, is_blocked, is_enabled, verification_token)
VALUES
    (1, 'user1', 'password1', 0, true, 'user1@example.com', false, true, 'token1'),
    (16, 'user2', 'password2', 1, true, 'user2@example.com', false, true, 'token2'),
    (2, 'user3', 'password3', 2, true, 'user3@example.com', false, true, 'token3');


INSERT INTO people (id, user_id, name, surname, email, profile_image, biography, quote, xp, level)
VALUES
    (16, 16, 'John', 'Doe', 'john@example.com', 'profile1.jpg', 'Biography of John Doe', 'I am John Doe', 100, 5),
    (1, 1, 'Alice', 'Smith', 'alice@example.com', 'profile2.jpg', 'Biography of Alice Smith', 'I am Alice Smith', 150, 6),
    (2, 2, 'Bob', 'Johnson', 'bob@example.com', 'profile3.jpg', 'Biography of Bob Johnson', 'I am Bob Johnson', 200, 7);


