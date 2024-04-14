package repo

import (
	"context"
	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
)

type FollowerRepository struct {
	Driver neo4j.DriverWithContext
}

func (repo *FollowerRepository) CreateFollowingCon(id1, id2 int) error {
	ctx := context.Background()
	session := repo.Driver.NewSession(ctx, neo4j.SessionConfig{DatabaseName: "neo4j"})
	defer session.Close(ctx)

	parameters := map[string]interface{}{
		"id1": id1,
		"id2": id2,
	}
	_, err := session.ExecuteWrite(ctx,
		func(transaction neo4j.ManagedTransaction) (any, error) {
			result, err := transaction.Run(
				ctx,
				`WITH $id1 as id1, $id2 as id2
						MERGE (u1: User {id: id1})
						MERGE (u2: User {id: id2})
						CREATE (u1) -[:Following]->(u2)
						RETURN u1, u2`,
				parameters)
			return result, err
		})
	return err
}

func (repo *FollowerRepository) IsFollowing(id1, id2 int) (bool, error) {
	ctx := context.Background()
	session := repo.Driver.NewSession(ctx, neo4j.SessionConfig{DatabaseName: "neo4j"})
	defer session.Close(ctx)

	parameters := map[string]interface{}{
		"id1": id1,
		"id2": id2,
	}

	result, err := session.ExecuteRead(ctx,
		func(transcation neo4j.ManagedTransaction) (any, error) {
			result, err := transcation.Run(
				ctx,
				`MATCH (u1: User{id: $id1}) -[f:Following]-> (u2: User {id: $id2})
						RETURN count(f) as count`,
				parameters)
			if err != nil {
				return 0, err
			}

			var value any
			if result.Next(ctx) {
				value, _ = result.Record().Get("count")
			}
			return value, nil

		})
	if err != nil {
		return false, err
	}

	return result.(int64) > 0, nil
}

func (repo *FollowerRepository) GetRecommendation(id int) ([]int, error) {

	ctx := context.Background()
	session := repo.Driver.NewSession(ctx, neo4j.SessionConfig{DatabaseName: "neo4j"})
	defer session.Close(ctx)

	parameters := map[string]interface{}{
		"id": id,
	}

	var recommendations []int
	result, err := session.ExecuteRead(ctx,
		func(transcation neo4j.ManagedTransaction) (any, error) {
			result, err := transcation.Run(ctx,
				`MATCH (u1:User {id: $id})-[:Following]->(u2:User)-[:Following]->(u3:User)
						WHERE NOT (u3)-[:Following]->(u1) AND u3.id <> $id
				 		RETURN DISTINCT u3.id as recommendationID`,
				parameters)
			if err != nil {
				return nil, err
			}
			for result.Next(ctx) {
				value, _ := result.Record().Get("recommendationID")
				recommendationId, _ := value.(int64)
				recommendations = append(recommendations, int(recommendationId))

			}
			return recommendations, nil
		},
	)
	if err != nil {
		return nil, err
	}
	return result.([]int), nil

}
