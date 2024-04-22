FROM golang:alpine

WORKDIR /go

COPY go.mod .
COPY go.sum .
RUN go mod download
COPY . .
RUN go build -o ./build/follower
EXPOSE 8095
ENTRYPOINT ["./build/follower"]
