package utils

type Path struct {
	Path string
	Role string // Roles {tourist, author, administrator} ; leave empty for all roles
}

func GetProtectedPaths() []*Path {
	return []*Path{
		{
			Path: "/tours/author/{UserId}",
			Role: "",
		},
		{
			Path: "/tours/{Id}",
			Role: "",
		},
		{
			Path: "/api/tourist/positionSimulator",
			Role: "",
		},
	}
}
