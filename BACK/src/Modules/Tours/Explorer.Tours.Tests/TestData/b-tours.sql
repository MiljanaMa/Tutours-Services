INSERT INTO tours."Tours"("Id", "UserId", "Name", "Description", "Price", "Duration", "Distance", "Difficulty",
                          "TransportType", "Status", "Tags", "StatusUpdateTime")
VALUES (-1, 1, 'TestTour', 'Wow, epic', 1234, 83, 2.3, 1, 1, 1, ARRAY[('Adventure', 'Epic')], '2023-05-20 12:07:18-09');
INSERT INTO tours."Tours"("Id", "UserId", "Name", "Description", "Price", "Duration", "Distance", "Difficulty",
                          "TransportType", "Status", "Tags", "StatusUpdateTime")
VALUES (-2, 1, 'TestTour2', 'ok', 12000, 33, 1.3, 1, 1, 1, ARRAY[('cringe', 'kinda')], '2023-05-20 12:07:18-09');
INSERT INTO tours."Tours"("Id", "UserId", "Name", "Description", "Price", "Duration", "Distance", "Difficulty",
                          "TransportType", "Status", "Tags", "StatusUpdateTime")
VALUES (-3, 1, 'TestTour3', 'not ok', 12342, 102, 0.5, 1, 1, 1, ARRAY[('Wow', 'fable')], '2023-05-20 12:07:18-09');