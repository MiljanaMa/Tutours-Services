-- Data for public.encounters
INSERT INTO encounters (
    id, user_id, name, description, latitude, longitude, xp, status, type, range, image, image_latitude, image_longitude, people_count, approval_status
) VALUES 
    (1, 2, 'enc3', 'aaa', 45.2427447333, 19.83357444, 30, 'ACTIVE', 'SOCIAL', 2, 'slika', 44.44, 44.44, 3, 'ADMIN_APPROVED'),
    (2, 1, 'enc2', 'aaa', 45.236821939947, 19.833578610326, 30, 'ACTIVE', 'LOCATION', 2, 'slika', 44.44, 44.44, 3, 'ADMIN_APPROVED'),
    (3, 1, 'enc1', 'aaa', 45.2427447597464, 19.8465677242953, 30, 'ACTIVE', 'MISC', 2, 'slika', 44.44, 44.44, 3, 'ADMIN_APPROVED'),
    (4, 1, 'enc4', 'aaa', 45.2427447333, 19.833578610326, 22, 'ACTIVE', 'MISC', 3, 'aanema', 45.22, 19.81, 4, 'ADMIN_APPROVED');

-- Data for public.keypoint_encounters
INSERT INTO keypoint_encounters (
    id, encounter_id, key_point_id, is_required
) VALUES 
    ('5accc143-0681-490b-8080-b805780ae858', 1, 3, 'f'),
    ('9accc143-0681-490b-8080-b805780ae858', 2, 3, 'f');

INSERT INTO encounter_completions (
    id, user_id, encounter_id, last_updated_at, xp, status
) VALUES 
    ('c3938ded-455f-4996-a8df-443394e48090', 1, 1, '2024-03-20 10:22:47.791878+01', 30, 'STARTED'),
    ('5accc143-0681-490b-8080-b805780ae858', 2, 2, '2024-03-20 10:23:31.652522+01', 30, 'STARTED'),
    ('6e0bdda9-8219-41aa-b89e-bc78cd95ebd0', 1, 3, '2024-03-20 10:21:55.57491+01', 30, 'COMPLETED');

