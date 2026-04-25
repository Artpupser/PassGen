name: Pull Request
description: Describe the changes and purpose of this PR

body:
  - type: textarea
    id: description
    attributes:
      label: Description
      description: What does this PR do and why?
    validations:
      required: true

  - type: input
    id: related_issue
    attributes:
      label: Related Issue
      description: Reference the issue (e.g., Closes #123)

  - type: checkboxes
    id: type_of_change
    attributes:
      label: Type of Change
      options:
        - label: Bug fix
        - label: New feature
        - label: Refactoring
        - label: Documentation update
        - label: Security improvement
        - label: Performance improvement

  - type: checkboxes
    id: testing
    attributes:
      label: How Has This Been Tested?
      options:
        - label: Linux
        - label: Windows
        - label: macOS

  - type: textarea
    id: test_commands
    attributes:
      label: Commands used to test
      render: bash

  - type: checkboxes
    id: checklist
    attributes:
      label: Checklist
      options:
        - label: Code follows the project's style guidelines
        - label: README updated if needed
        - label: No new security vulnerabilities introduced

  - type: textarea
    id: screenshots
    attributes:
      label: Screenshots / Output
      description: Attach terminal output or before/after screenshots
