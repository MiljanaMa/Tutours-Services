package helper

import (
	"database/sql/driver"
	"fmt"
	"strings"
)

type ArrayString []string

// Value converts ArrayString to a PostgreSQL-compatible representation
func (a ArrayString) Value() (driver.Value, error) {
	if len(a) == 0 {
		return "{}", nil
	}

	// Convert array elements to a string slice
	strSlice := make([]string, len(a))
	for i, v := range a {
		strSlice[i] = fmt.Sprintf("%s", v)
	}

	// Join elements with commas and enclose in curly braces
	result := fmt.Sprintf("{%s}", strings.Join(strSlice, ","))

	return result, nil
}

// Scan converts a PostgreSQL array string representation to ArrayString
func (a *ArrayString) Scan(value interface{}) error {
	if value == nil {
		*a = nil
		return nil
	}

	// Convert value to string
	str, ok := value.(string)
	if !ok {
		return fmt.Errorf("unsupported type for ArrayString: %T", value)
	}

	// Remove curly braces from the string
	str = strings.TrimPrefix(str, "{")
	str = strings.TrimSuffix(str, "}")

	// Split string into individual tags
	tags := strings.Split(str, ",")

	// Trim whitespaces from each tag
	for i, tag := range tags {
		tags[i] = strings.TrimSpace(tag)
	}

	*a = ArrayString(tags)
	return nil
}
