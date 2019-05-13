.RECIPEPREFIX +=

CC := g++
LIB_DIR := lib
TEST_DIR := test
BUILD_DIR := build
INCLUDE_DIR := include $(LIB_DIR)
TEST_TARGET := test/test_main

SRC_EXT := cc
SRCS := $(shell find $(LIB_DIR) -type f -name *.$(SRC_EXT))
TEST_SRCS := $(shell find $(TEST_DIR) -type f -name *.$(SRC_EXT))
ALL_SRCS := $(SRCS) $(TEST_SRCS)
OBJS := $(ALL_SRCS:%.$(SRC_EXT)=$(BUILD_DIR)/%.o)
DEPS := $(OBJS:%.o=%.d)
 
CFLAGS := -std=c++17 -g -Wall -Wextra $(INCLUDE_DIR:%=-I %)

test: $(TEST_TARGET)
  $(info Successfully built $(TEST_TARGET).)

clean:
  $(info Removing $(BUILD_DIR) directory.)
  @$(RM) -rf $(BUILD_DIR)
  $(info Removing $(TEST_TARGET).)
  @$(RM) -f $(TEST_TARGET)

.PHONY: test clean

$(TEST_TARGET): $(OBJS)
  $(CC) $(CFLAGS) -o $@ $(OBJS)

$(OBJS) : $(BUILD_DIR)/%.o : %.$(SRC_EXT)
  @mkdir -p $(@D)
  $(CC) $(CFLAGS) -c -o $@ $^

-include $(DEPS)