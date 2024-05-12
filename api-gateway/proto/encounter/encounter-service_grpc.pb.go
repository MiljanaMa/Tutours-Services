// Code generated by protoc-gen-go-grpc. DO NOT EDIT.
// versions:
// - protoc-gen-go-grpc v1.3.0
// - protoc             v5.26.1
// source: encounter/encounter-service.proto

package encounter

import (
	context "context"
	grpc "google.golang.org/grpc"
	codes "google.golang.org/grpc/codes"
	status "google.golang.org/grpc/status"
)

// This is a compile-time assertion to ensure that this generated file
// is compatible with the grpc package it is being compiled against.
// Requires gRPC-Go v1.32.0 or later.
const _ = grpc.SupportPackageIsVersion7

const (
	EncounterService_GetApproved_FullMethodName         = "/encounter.EncounterService/GetApproved"
	EncounterService_GetTouristCreated_FullMethodName   = "/encounter.EncounterService/GetTouristCreated"
	EncounterService_GetByUser_FullMethodName           = "/encounter.EncounterService/GetByUser"
	EncounterService_GetApprovedByStatus_FullMethodName = "/encounter.EncounterService/GetApprovedByStatus"
	EncounterService_GetNearby_FullMethodName           = "/encounter.EncounterService/GetNearby"
	EncounterService_GetNearbyByType_FullMethodName     = "/encounter.EncounterService/GetNearbyByType"
	EncounterService_CreateEncounter_FullMethodName     = "/encounter.EncounterService/CreateEncounter"
	EncounterService_UpdateEncounter_FullMethodName     = "/encounter.EncounterService/UpdateEncounter"
	EncounterService_Approve_FullMethodName             = "/encounter.EncounterService/Approve"
	EncounterService_Decline_FullMethodName             = "/encounter.EncounterService/Decline"
	EncounterService_DeleteEncounter_FullMethodName     = "/encounter.EncounterService/DeleteEncounter"
)

// EncounterServiceClient is the client API for EncounterService service.
//
// For semantics around ctx use and closing/ending streaming RPCs, please refer to https://pkg.go.dev/google.golang.org/grpc/?tab=doc#ClientConn.NewStream.
type EncounterServiceClient interface {
	GetApproved(ctx context.Context, in *EmptyRequest, opts ...grpc.CallOption) (*EncountersResponse, error)
	GetTouristCreated(ctx context.Context, in *EmptyRequest, opts ...grpc.CallOption) (*EncountersResponse, error)
	GetByUser(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncountersResponse, error)
	GetApprovedByStatus(ctx context.Context, in *StatusRequest, opts ...grpc.CallOption) (*EncountersResponse, error)
	GetNearby(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncountersResponse, error)
	GetNearbyByType(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncountersResponse, error)
	CreateEncounter(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*Encounter, error)
	UpdateEncounter(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*Encounter, error)
	Approve(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*EmptyResponse, error)
	Decline(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*EmptyResponse, error)
	DeleteEncounter(ctx context.Context, in *IdRequest, opts ...grpc.CallOption) (*EmptyResponse, error)
}

type encounterServiceClient struct {
	cc grpc.ClientConnInterface
}

func NewEncounterServiceClient(cc grpc.ClientConnInterface) EncounterServiceClient {
	return &encounterServiceClient{cc}
}

func (c *encounterServiceClient) GetApproved(ctx context.Context, in *EmptyRequest, opts ...grpc.CallOption) (*EncountersResponse, error) {
	out := new(EncountersResponse)
	err := c.cc.Invoke(ctx, EncounterService_GetApproved_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) GetTouristCreated(ctx context.Context, in *EmptyRequest, opts ...grpc.CallOption) (*EncountersResponse, error) {
	out := new(EncountersResponse)
	err := c.cc.Invoke(ctx, EncounterService_GetTouristCreated_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) GetByUser(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncountersResponse, error) {
	out := new(EncountersResponse)
	err := c.cc.Invoke(ctx, EncounterService_GetByUser_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) GetApprovedByStatus(ctx context.Context, in *StatusRequest, opts ...grpc.CallOption) (*EncountersResponse, error) {
	out := new(EncountersResponse)
	err := c.cc.Invoke(ctx, EncounterService_GetApprovedByStatus_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) GetNearby(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncountersResponse, error) {
	out := new(EncountersResponse)
	err := c.cc.Invoke(ctx, EncounterService_GetNearby_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) GetNearbyByType(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncountersResponse, error) {
	out := new(EncountersResponse)
	err := c.cc.Invoke(ctx, EncounterService_GetNearbyByType_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) CreateEncounter(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*Encounter, error) {
	out := new(Encounter)
	err := c.cc.Invoke(ctx, EncounterService_CreateEncounter_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) UpdateEncounter(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*Encounter, error) {
	out := new(Encounter)
	err := c.cc.Invoke(ctx, EncounterService_UpdateEncounter_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) Approve(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*EmptyResponse, error) {
	out := new(EmptyResponse)
	err := c.cc.Invoke(ctx, EncounterService_Approve_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) Decline(ctx context.Context, in *Encounter, opts ...grpc.CallOption) (*EmptyResponse, error) {
	out := new(EmptyResponse)
	err := c.cc.Invoke(ctx, EncounterService_Decline_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterServiceClient) DeleteEncounter(ctx context.Context, in *IdRequest, opts ...grpc.CallOption) (*EmptyResponse, error) {
	out := new(EmptyResponse)
	err := c.cc.Invoke(ctx, EncounterService_DeleteEncounter_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

// EncounterServiceServer is the server API for EncounterService service.
// All implementations must embed UnimplementedEncounterServiceServer
// for forward compatibility
type EncounterServiceServer interface {
	GetApproved(context.Context, *EmptyRequest) (*EncountersResponse, error)
	GetTouristCreated(context.Context, *EmptyRequest) (*EncountersResponse, error)
	GetByUser(context.Context, *UserIdRequest) (*EncountersResponse, error)
	GetApprovedByStatus(context.Context, *StatusRequest) (*EncountersResponse, error)
	GetNearby(context.Context, *UserIdRequest) (*EncountersResponse, error)
	GetNearbyByType(context.Context, *UserIdRequest) (*EncountersResponse, error)
	CreateEncounter(context.Context, *Encounter) (*Encounter, error)
	UpdateEncounter(context.Context, *Encounter) (*Encounter, error)
	Approve(context.Context, *Encounter) (*EmptyResponse, error)
	Decline(context.Context, *Encounter) (*EmptyResponse, error)
	DeleteEncounter(context.Context, *IdRequest) (*EmptyResponse, error)
	mustEmbedUnimplementedEncounterServiceServer()
}

// UnimplementedEncounterServiceServer must be embedded to have forward compatible implementations.
type UnimplementedEncounterServiceServer struct {
}

func (UnimplementedEncounterServiceServer) GetApproved(context.Context, *EmptyRequest) (*EncountersResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetApproved not implemented")
}
func (UnimplementedEncounterServiceServer) GetTouristCreated(context.Context, *EmptyRequest) (*EncountersResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetTouristCreated not implemented")
}
func (UnimplementedEncounterServiceServer) GetByUser(context.Context, *UserIdRequest) (*EncountersResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetByUser not implemented")
}
func (UnimplementedEncounterServiceServer) GetApprovedByStatus(context.Context, *StatusRequest) (*EncountersResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetApprovedByStatus not implemented")
}
func (UnimplementedEncounterServiceServer) GetNearby(context.Context, *UserIdRequest) (*EncountersResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetNearby not implemented")
}
func (UnimplementedEncounterServiceServer) GetNearbyByType(context.Context, *UserIdRequest) (*EncountersResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetNearbyByType not implemented")
}
func (UnimplementedEncounterServiceServer) CreateEncounter(context.Context, *Encounter) (*Encounter, error) {
	return nil, status.Errorf(codes.Unimplemented, "method CreateEncounter not implemented")
}
func (UnimplementedEncounterServiceServer) UpdateEncounter(context.Context, *Encounter) (*Encounter, error) {
	return nil, status.Errorf(codes.Unimplemented, "method UpdateEncounter not implemented")
}
func (UnimplementedEncounterServiceServer) Approve(context.Context, *Encounter) (*EmptyResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method Approve not implemented")
}
func (UnimplementedEncounterServiceServer) Decline(context.Context, *Encounter) (*EmptyResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method Decline not implemented")
}
func (UnimplementedEncounterServiceServer) DeleteEncounter(context.Context, *IdRequest) (*EmptyResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method DeleteEncounter not implemented")
}
func (UnimplementedEncounterServiceServer) mustEmbedUnimplementedEncounterServiceServer() {}

// UnsafeEncounterServiceServer may be embedded to opt out of forward compatibility for this service.
// Use of this interface is not recommended, as added methods to EncounterServiceServer will
// result in compilation errors.
type UnsafeEncounterServiceServer interface {
	mustEmbedUnimplementedEncounterServiceServer()
}

func RegisterEncounterServiceServer(s grpc.ServiceRegistrar, srv EncounterServiceServer) {
	s.RegisterService(&EncounterService_ServiceDesc, srv)
}

func _EncounterService_GetApproved_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(EmptyRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).GetApproved(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_GetApproved_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).GetApproved(ctx, req.(*EmptyRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_GetTouristCreated_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(EmptyRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).GetTouristCreated(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_GetTouristCreated_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).GetTouristCreated(ctx, req.(*EmptyRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_GetByUser_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(UserIdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).GetByUser(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_GetByUser_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).GetByUser(ctx, req.(*UserIdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_GetApprovedByStatus_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(StatusRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).GetApprovedByStatus(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_GetApprovedByStatus_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).GetApprovedByStatus(ctx, req.(*StatusRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_GetNearby_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(UserIdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).GetNearby(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_GetNearby_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).GetNearby(ctx, req.(*UserIdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_GetNearbyByType_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(UserIdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).GetNearbyByType(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_GetNearbyByType_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).GetNearbyByType(ctx, req.(*UserIdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_CreateEncounter_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(Encounter)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).CreateEncounter(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_CreateEncounter_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).CreateEncounter(ctx, req.(*Encounter))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_UpdateEncounter_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(Encounter)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).UpdateEncounter(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_UpdateEncounter_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).UpdateEncounter(ctx, req.(*Encounter))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_Approve_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(Encounter)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).Approve(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_Approve_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).Approve(ctx, req.(*Encounter))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_Decline_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(Encounter)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).Decline(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_Decline_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).Decline(ctx, req.(*Encounter))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterService_DeleteEncounter_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(IdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterServiceServer).DeleteEncounter(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterService_DeleteEncounter_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterServiceServer).DeleteEncounter(ctx, req.(*IdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

// EncounterService_ServiceDesc is the grpc.ServiceDesc for EncounterService service.
// It's only intended for direct use with grpc.RegisterService,
// and not to be introspected or modified (even as a copy)
var EncounterService_ServiceDesc = grpc.ServiceDesc{
	ServiceName: "encounter.EncounterService",
	HandlerType: (*EncounterServiceServer)(nil),
	Methods: []grpc.MethodDesc{
		{
			MethodName: "GetApproved",
			Handler:    _EncounterService_GetApproved_Handler,
		},
		{
			MethodName: "GetTouristCreated",
			Handler:    _EncounterService_GetTouristCreated_Handler,
		},
		{
			MethodName: "GetByUser",
			Handler:    _EncounterService_GetByUser_Handler,
		},
		{
			MethodName: "GetApprovedByStatus",
			Handler:    _EncounterService_GetApprovedByStatus_Handler,
		},
		{
			MethodName: "GetNearby",
			Handler:    _EncounterService_GetNearby_Handler,
		},
		{
			MethodName: "GetNearbyByType",
			Handler:    _EncounterService_GetNearbyByType_Handler,
		},
		{
			MethodName: "CreateEncounter",
			Handler:    _EncounterService_CreateEncounter_Handler,
		},
		{
			MethodName: "UpdateEncounter",
			Handler:    _EncounterService_UpdateEncounter_Handler,
		},
		{
			MethodName: "Approve",
			Handler:    _EncounterService_Approve_Handler,
		},
		{
			MethodName: "Decline",
			Handler:    _EncounterService_Decline_Handler,
		},
		{
			MethodName: "DeleteEncounter",
			Handler:    _EncounterService_DeleteEncounter_Handler,
		},
	},
	Streams:  []grpc.StreamDesc{},
	Metadata: "encounter/encounter-service.proto",
}

const (
	EncounterCompletionService_GetEncounterCompletionByUser_FullMethodName = "/encounter.EncounterCompletionService/GetEncounterCompletionByUser"
	EncounterCompletionService_FinishEncounter_FullMethodName              = "/encounter.EncounterCompletionService/FinishEncounter"
)

// EncounterCompletionServiceClient is the client API for EncounterCompletionService service.
//
// For semantics around ctx use and closing/ending streaming RPCs, please refer to https://pkg.go.dev/google.golang.org/grpc/?tab=doc#ClientConn.NewStream.
type EncounterCompletionServiceClient interface {
	GetEncounterCompletionByUser(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncounterCompletionsResponse, error)
	FinishEncounter(ctx context.Context, in *UserAndIdRequest, opts ...grpc.CallOption) (*EncounterCompletion, error)
}

type encounterCompletionServiceClient struct {
	cc grpc.ClientConnInterface
}

func NewEncounterCompletionServiceClient(cc grpc.ClientConnInterface) EncounterCompletionServiceClient {
	return &encounterCompletionServiceClient{cc}
}

func (c *encounterCompletionServiceClient) GetEncounterCompletionByUser(ctx context.Context, in *UserIdRequest, opts ...grpc.CallOption) (*EncounterCompletionsResponse, error) {
	out := new(EncounterCompletionsResponse)
	err := c.cc.Invoke(ctx, EncounterCompletionService_GetEncounterCompletionByUser_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *encounterCompletionServiceClient) FinishEncounter(ctx context.Context, in *UserAndIdRequest, opts ...grpc.CallOption) (*EncounterCompletion, error) {
	out := new(EncounterCompletion)
	err := c.cc.Invoke(ctx, EncounterCompletionService_FinishEncounter_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

// EncounterCompletionServiceServer is the server API for EncounterCompletionService service.
// All implementations must embed UnimplementedEncounterCompletionServiceServer
// for forward compatibility
type EncounterCompletionServiceServer interface {
	GetEncounterCompletionByUser(context.Context, *UserIdRequest) (*EncounterCompletionsResponse, error)
	FinishEncounter(context.Context, *UserAndIdRequest) (*EncounterCompletion, error)
	mustEmbedUnimplementedEncounterCompletionServiceServer()
}

// UnimplementedEncounterCompletionServiceServer must be embedded to have forward compatible implementations.
type UnimplementedEncounterCompletionServiceServer struct {
}

func (UnimplementedEncounterCompletionServiceServer) GetEncounterCompletionByUser(context.Context, *UserIdRequest) (*EncounterCompletionsResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetEncounterCompletionByUser not implemented")
}
func (UnimplementedEncounterCompletionServiceServer) FinishEncounter(context.Context, *UserAndIdRequest) (*EncounterCompletion, error) {
	return nil, status.Errorf(codes.Unimplemented, "method FinishEncounter not implemented")
}
func (UnimplementedEncounterCompletionServiceServer) mustEmbedUnimplementedEncounterCompletionServiceServer() {
}

// UnsafeEncounterCompletionServiceServer may be embedded to opt out of forward compatibility for this service.
// Use of this interface is not recommended, as added methods to EncounterCompletionServiceServer will
// result in compilation errors.
type UnsafeEncounterCompletionServiceServer interface {
	mustEmbedUnimplementedEncounterCompletionServiceServer()
}

func RegisterEncounterCompletionServiceServer(s grpc.ServiceRegistrar, srv EncounterCompletionServiceServer) {
	s.RegisterService(&EncounterCompletionService_ServiceDesc, srv)
}

func _EncounterCompletionService_GetEncounterCompletionByUser_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(UserIdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterCompletionServiceServer).GetEncounterCompletionByUser(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterCompletionService_GetEncounterCompletionByUser_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterCompletionServiceServer).GetEncounterCompletionByUser(ctx, req.(*UserIdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _EncounterCompletionService_FinishEncounter_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(UserAndIdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(EncounterCompletionServiceServer).FinishEncounter(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: EncounterCompletionService_FinishEncounter_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(EncounterCompletionServiceServer).FinishEncounter(ctx, req.(*UserAndIdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

// EncounterCompletionService_ServiceDesc is the grpc.ServiceDesc for EncounterCompletionService service.
// It's only intended for direct use with grpc.RegisterService,
// and not to be introspected or modified (even as a copy)
var EncounterCompletionService_ServiceDesc = grpc.ServiceDesc{
	ServiceName: "encounter.EncounterCompletionService",
	HandlerType: (*EncounterCompletionServiceServer)(nil),
	Methods: []grpc.MethodDesc{
		{
			MethodName: "GetEncounterCompletionByUser",
			Handler:    _EncounterCompletionService_GetEncounterCompletionByUser_Handler,
		},
		{
			MethodName: "FinishEncounter",
			Handler:    _EncounterCompletionService_FinishEncounter_Handler,
		},
	},
	Streams:  []grpc.StreamDesc{},
	Metadata: "encounter/encounter-service.proto",
}

const (
	KeypointEncounterService_GetByKeypoint_FullMethodName = "/encounter.KeypointEncounterService/GetByKeypoint"
	KeypointEncounterService_Create_FullMethodName        = "/encounter.KeypointEncounterService/Create"
	KeypointEncounterService_Update_FullMethodName        = "/encounter.KeypointEncounterService/Update"
	KeypointEncounterService_Delete_FullMethodName        = "/encounter.KeypointEncounterService/Delete"
)

// KeypointEncounterServiceClient is the client API for KeypointEncounterService service.
//
// For semantics around ctx use and closing/ending streaming RPCs, please refer to https://pkg.go.dev/google.golang.org/grpc/?tab=doc#ClientConn.NewStream.
type KeypointEncounterServiceClient interface {
	GetByKeypoint(ctx context.Context, in *IdRequest, opts ...grpc.CallOption) (*KeypointEncounterResponse, error)
	Create(ctx context.Context, in *KeypointEncounter, opts ...grpc.CallOption) (*KeypointEncounter, error)
	Update(ctx context.Context, in *KeypointEncounter, opts ...grpc.CallOption) (*KeypointEncounter, error)
	Delete(ctx context.Context, in *IdRequest, opts ...grpc.CallOption) (*EmptyResponse, error)
}

type keypointEncounterServiceClient struct {
	cc grpc.ClientConnInterface
}

func NewKeypointEncounterServiceClient(cc grpc.ClientConnInterface) KeypointEncounterServiceClient {
	return &keypointEncounterServiceClient{cc}
}

func (c *keypointEncounterServiceClient) GetByKeypoint(ctx context.Context, in *IdRequest, opts ...grpc.CallOption) (*KeypointEncounterResponse, error) {
	out := new(KeypointEncounterResponse)
	err := c.cc.Invoke(ctx, KeypointEncounterService_GetByKeypoint_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *keypointEncounterServiceClient) Create(ctx context.Context, in *KeypointEncounter, opts ...grpc.CallOption) (*KeypointEncounter, error) {
	out := new(KeypointEncounter)
	err := c.cc.Invoke(ctx, KeypointEncounterService_Create_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *keypointEncounterServiceClient) Update(ctx context.Context, in *KeypointEncounter, opts ...grpc.CallOption) (*KeypointEncounter, error) {
	out := new(KeypointEncounter)
	err := c.cc.Invoke(ctx, KeypointEncounterService_Update_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *keypointEncounterServiceClient) Delete(ctx context.Context, in *IdRequest, opts ...grpc.CallOption) (*EmptyResponse, error) {
	out := new(EmptyResponse)
	err := c.cc.Invoke(ctx, KeypointEncounterService_Delete_FullMethodName, in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

// KeypointEncounterServiceServer is the server API for KeypointEncounterService service.
// All implementations must embed UnimplementedKeypointEncounterServiceServer
// for forward compatibility
type KeypointEncounterServiceServer interface {
	GetByKeypoint(context.Context, *IdRequest) (*KeypointEncounterResponse, error)
	Create(context.Context, *KeypointEncounter) (*KeypointEncounter, error)
	Update(context.Context, *KeypointEncounter) (*KeypointEncounter, error)
	Delete(context.Context, *IdRequest) (*EmptyResponse, error)
	mustEmbedUnimplementedKeypointEncounterServiceServer()
}

// UnimplementedKeypointEncounterServiceServer must be embedded to have forward compatible implementations.
type UnimplementedKeypointEncounterServiceServer struct {
}

func (UnimplementedKeypointEncounterServiceServer) GetByKeypoint(context.Context, *IdRequest) (*KeypointEncounterResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetByKeypoint not implemented")
}
func (UnimplementedKeypointEncounterServiceServer) Create(context.Context, *KeypointEncounter) (*KeypointEncounter, error) {
	return nil, status.Errorf(codes.Unimplemented, "method Create not implemented")
}
func (UnimplementedKeypointEncounterServiceServer) Update(context.Context, *KeypointEncounter) (*KeypointEncounter, error) {
	return nil, status.Errorf(codes.Unimplemented, "method Update not implemented")
}
func (UnimplementedKeypointEncounterServiceServer) Delete(context.Context, *IdRequest) (*EmptyResponse, error) {
	return nil, status.Errorf(codes.Unimplemented, "method Delete not implemented")
}
func (UnimplementedKeypointEncounterServiceServer) mustEmbedUnimplementedKeypointEncounterServiceServer() {
}

// UnsafeKeypointEncounterServiceServer may be embedded to opt out of forward compatibility for this service.
// Use of this interface is not recommended, as added methods to KeypointEncounterServiceServer will
// result in compilation errors.
type UnsafeKeypointEncounterServiceServer interface {
	mustEmbedUnimplementedKeypointEncounterServiceServer()
}

func RegisterKeypointEncounterServiceServer(s grpc.ServiceRegistrar, srv KeypointEncounterServiceServer) {
	s.RegisterService(&KeypointEncounterService_ServiceDesc, srv)
}

func _KeypointEncounterService_GetByKeypoint_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(IdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(KeypointEncounterServiceServer).GetByKeypoint(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: KeypointEncounterService_GetByKeypoint_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(KeypointEncounterServiceServer).GetByKeypoint(ctx, req.(*IdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _KeypointEncounterService_Create_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(KeypointEncounter)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(KeypointEncounterServiceServer).Create(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: KeypointEncounterService_Create_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(KeypointEncounterServiceServer).Create(ctx, req.(*KeypointEncounter))
	}
	return interceptor(ctx, in, info, handler)
}

func _KeypointEncounterService_Update_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(KeypointEncounter)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(KeypointEncounterServiceServer).Update(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: KeypointEncounterService_Update_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(KeypointEncounterServiceServer).Update(ctx, req.(*KeypointEncounter))
	}
	return interceptor(ctx, in, info, handler)
}

func _KeypointEncounterService_Delete_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(IdRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(KeypointEncounterServiceServer).Delete(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: KeypointEncounterService_Delete_FullMethodName,
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(KeypointEncounterServiceServer).Delete(ctx, req.(*IdRequest))
	}
	return interceptor(ctx, in, info, handler)
}

// KeypointEncounterService_ServiceDesc is the grpc.ServiceDesc for KeypointEncounterService service.
// It's only intended for direct use with grpc.RegisterService,
// and not to be introspected or modified (even as a copy)
var KeypointEncounterService_ServiceDesc = grpc.ServiceDesc{
	ServiceName: "encounter.KeypointEncounterService",
	HandlerType: (*KeypointEncounterServiceServer)(nil),
	Methods: []grpc.MethodDesc{
		{
			MethodName: "GetByKeypoint",
			Handler:    _KeypointEncounterService_GetByKeypoint_Handler,
		},
		{
			MethodName: "Create",
			Handler:    _KeypointEncounterService_Create_Handler,
		},
		{
			MethodName: "Update",
			Handler:    _KeypointEncounterService_Update_Handler,
		},
		{
			MethodName: "Delete",
			Handler:    _KeypointEncounterService_Delete_Handler,
		},
	},
	Streams:  []grpc.StreamDesc{},
	Metadata: "encounter/encounter-service.proto",
}