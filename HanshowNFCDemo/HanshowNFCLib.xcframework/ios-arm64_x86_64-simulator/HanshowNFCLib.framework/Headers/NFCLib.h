//
//  HanshowNFCLib.h
//
//
//  Created by hanshow on 2023/5/16.
//

#import <Foundation/Foundation.h>
#import <CoreNFC/CoreNFC.h>
#import <UIKit/UIKit.h>
#import <HanshowNFCLib/NFCResponse.h>

@interface NFCLib : NSObject

/**
 Returns the shared instance of the NFCLib object. This instance is often used in the singleton pattern.
 */
+ (instancetype )sharedInstance;

/**
 Checks if the provided NFC tag is supported by the library.
 @param tag The NFC tag to check.
 @return Returns true if the tag is supported, false otherwise.
 */
+ (bool)isSupportedTag:(id<NFCTag>)tag;

/**
 Initiates an action to get the ID of the provided ESL (Electronic Shelf Label) tag.
 @param tag The ESL tag for which the ID is requested.
 @param finishBlock A block that is called when the action is completed. The block takes a NFCResponse object as its parameter, which contains the result of the action.
 */
- (void)getEslIdAction:(id<NFCTag>)tag FinishBlock:(void(^)(NFCResponse *response))finishBlock;

@end


