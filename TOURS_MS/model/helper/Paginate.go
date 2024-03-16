package helper

import "gorm.io/gorm"

type Paginate struct {
	limit int
	page  int
}

func NewPaginate(limit int, page int) *Paginate {
	return &Paginate{limit: limit, page: page}
}

func (p *Paginate) PaginatedResult(db *gorm.DB) *gorm.DB {
	offset := (p.page - 1) * p.limit

	return db.Offset(offset).
		Limit(p.limit)
}
