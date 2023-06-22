//
//  NFCResponse.h
//  
//
//  Created by hanshow on 2023/5/19.
//

#import <Foundation/Foundation.h>
#import <HanshowNFCLib/NFCError.h>


@interface NFCResponse : NSObject
@property(nonatomic,strong) NSDictionary *data;
@property(nonatomic,strong) NFCError *error;
+(instancetype)responseWithData:(NSDictionary*)data Error:(NFCError*)error;

@end


