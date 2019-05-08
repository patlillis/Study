.RECIPEPREFIX +=

CC := g++
SRCDIR := src
BUILDDIR := build
TARGET := bin/main

SRCEXT := cc
SOURCES := $(shell find $(SRCDIR) -type f -name *.$(SRCEXT))
OBJECTS = $(patsubst $(SRCDIR)/%.$(SRCEXT),$(BUILDDIR)/%.o,$(SOURCES))
CFLAGS := -Wall -g

build: $(TARGET)

clean:
  $(RM) -rf $(BUILDDIR)
  $(RM) -f $(TARGET)

.PHONY: build clean

$(TARGET): $(OBJECTS)
  @mkdir -p $(@D)
  $(CC) -o $@ $^

$(OBJECTS): $(BUILDDIR)/%.o: $(SRCDIR)/%.$(SRCEXT)
  @mkdir -p $(@D)
  $(CC) $(CFLAGS) -c -o $@ $^
